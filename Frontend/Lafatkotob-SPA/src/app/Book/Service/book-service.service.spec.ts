import { TestBed } from '@angular/core/testing';

import { BookServiceService } from './BookService';

describe('BookServiceService', () => {
  let service: BookServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
