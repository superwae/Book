import { TestBed } from '@angular/core/testing';

import { AppUserServiceService } from './app-user-service.service';

describe('AppUserServiceService', () => {
  let service: AppUserServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppUserServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
