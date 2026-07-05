import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooksTable } from './books-table';

describe('BooksTable', () => {
  let component: BooksTable;
  let fixture: ComponentFixture<BooksTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BooksTable],
    }).compileComponents();

    fixture = TestBed.createComponent(BooksTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
