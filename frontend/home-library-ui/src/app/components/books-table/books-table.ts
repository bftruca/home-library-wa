import { Component, input } from '@angular/core';
import { DatePipe } from '@angular/common';

import { BookResponse } from '../../models/book-response';

@Component({
    selector: 'app-books-table',
    standalone: true,
    imports: [DatePipe],
    templateUrl: './books-table.html',
    styleUrls: ['./books-table.css']
})
export class BooksTable {
    readonly books = input.required<BookResponse[]>();
}