import { Component, inject, signal } from '@angular/core';
import { finalize, switchMap, take, timer } from 'rxjs';

import { BookResponse } from './models/book-response';
import { BooksService } from './services/books.service';
import { BooksTable } from './components/books-table/books-table';
import { FileUpload } from './components/file-upload/file-upload';
import { ImportResponse } from './models/import-response';

@Component({
  selector: 'app-root',
  imports: [
    BooksTable,
    FileUpload
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  private readonly booksService = inject(BooksService);

  readonly books = signal<BookResponse[]>([]);
  readonly isLoading = signal(false);

  constructor() {
    this.loadBooks();
  }

  onImportCompleted(response: ImportResponse): void {
    console.log(`${response.queued} books queued for import`);

    timer(500, 1000)
      .pipe(
        take(10),
        switchMap(() => this.booksService.getBooks())
      )
      .subscribe({
        next: books => {
          this.books.set(books);
        },
        error: error => {
          console.error('Could not refresh books:', error);
        }
      });
  }

  private loadBooks(): void {
    this.isLoading.set(true);

    this.booksService.getBooks()
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: books => {
          this.books.set(books);
        },
        error: error => {
          console.error('Could not load books:', error);
        }
      });
  }
}