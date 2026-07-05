import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  inject,
  output,
  signal
} from '@angular/core';
import { finalize } from 'rxjs';

import { ImportResponse } from '../../models/import-response';
import { ImportsService } from '../../services/imports.service';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [],
  templateUrl: './file-upload.html',
  styleUrl: './file-upload.css'
})

export class FileUpload {

  private readonly importsService = inject(ImportsService);

  private dragCounter = 0;

  readonly importCompleted = output<ImportResponse>();

  readonly selectedFile = signal<File | null>(null);
  readonly isDragging = signal(false);
  readonly isUploading = signal(false);

  readonly successMessage = signal<string | null>(null);
  readonly errorMessage = signal<string | null>(null);

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (file) {
      this.selectFile(file);
    }

    input.value = '';
  }

  onDragEnter(event: DragEvent): void {
  event.preventDefault();

  if (this.isUploading()) {
    return;
  }

  this.dragCounter++;
  this.isDragging.set(true);
}

onDragOver(event: DragEvent): void {
  event.preventDefault();

  if (event.dataTransfer) {
    event.dataTransfer.dropEffect = 'copy';
  }
}

onDragLeave(event: DragEvent): void {
  event.preventDefault();

  this.dragCounter--;

  if (this.dragCounter <= 0) {
    this.dragCounter = 0;
    this.isDragging.set(false);
  }
}

onDrop(event: DragEvent): void {
  event.preventDefault();

  this.dragCounter = 0;
  this.isDragging.set(false);

  if (this.isUploading()) {
    return;
  }

  const file = event.dataTransfer?.files[0];

  if (file) {
    this.selectFile(file);
  }
}

  removeFile(): void {
    if (this.isUploading()) {
      return;
    }

    this.selectedFile.set(null);
    this.successMessage.set(null);
    this.errorMessage.set(null);
  }

  importFile(): void {
    const fileName = this.selectedFile();

    if (!fileName || this.isUploading()) {
      return;
    }

    this.isUploading.set(true);
    this.successMessage.set(null);
    this.errorMessage.set(null);

    this.importsService.importBooks(fileName)
      .pipe(
        finalize(() => this.isUploading.set(false))
      )
      .subscribe({
        next: response => {
          this.successMessage.set(
            `${response.queued} books were queued for import.`
          );

          this.selectedFile.set(null);
          this.importCompleted.emit(response);
        },
        error: (error: HttpErrorResponse) => {
          console.error(error);

          this.errorMessage.set(
            'The CSV file could not be imported.'
          );
        }
      });
  }

  formatFileSize(sizeInBytes: number): string {
    const sizeInKb = sizeInBytes / 1024;

    if (sizeInKb < 1024) {
      return `${sizeInKb.toFixed(1)} KB`;
    }

    return `${(sizeInKb / 1024).toFixed(1)} MB`;
  }

  private selectFile(file: File): void {
    this.successMessage.set(null);
    this.errorMessage.set(null);

    if (!file.name.toLowerCase().endsWith('.csv')) {
      this.selectedFile.set(null);
      this.errorMessage.set('Only CSV files are accepted.');
      return;
    }

    if (file.size === 0) {
      this.selectedFile.set(null);
      this.errorMessage.set('The selected file is empty.');
      return;
    }

    this.selectedFile.set(file);
  }
}