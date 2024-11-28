import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-indicator',
  template: `
    <div *ngIf="isLoading" class="status-message loading">
      {{ message }}
    </div>
  `
})
export class LoadingIndicatorComponent {
  @Input() isLoading = false;
  @Input() message = 'Loading...';
} 