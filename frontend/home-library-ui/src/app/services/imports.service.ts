import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { ImportResponse } from '../models/import-response';

@Injectable({
  providedIn: 'root'
})
export class ImportsService {
  private readonly http = inject(HttpClient);

  importBooks(file: File): Observable<ImportResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<ImportResponse>(`${environment.apiUrl}/imports`, formData);
  }
}