/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TasinmazService } from './tasinmaz.service';

describe('Service: Tasinmaz', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TasinmazService]
    });
  });

  it('should ...', inject([TasinmazService], (service: TasinmazService) => {
    expect(service).toBeTruthy();
  }));
});
