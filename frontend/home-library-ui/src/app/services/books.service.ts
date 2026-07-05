import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { BookResponse } from '../models/book-response';

@Injectable({
  providedIn: 'root'
})
export class BooksService {
  private readonly http = inject(HttpClient);

  getBooks(): Observable<BookResponse[]> {
    return this.http.get<BookResponse[]>(`${environment.apiUrl}/books`);
  }
}