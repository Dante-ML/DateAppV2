import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  private spinnerService = inject(NgxSpinnerService);
  
  busy(): void {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: "ball-fall",
      bdColor: "rgba(2153,0,76,0.5)",
      color: "rgba(224,244,244,1)"
    });
  }
  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}