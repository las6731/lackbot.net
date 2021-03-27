import { TestBed } from '@angular/core/testing';

import { ScheduledMessagesService } from './scheduled-messages.service';

describe('ScheduledMessagesService', () => {
  let service: ScheduledMessagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScheduledMessagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
