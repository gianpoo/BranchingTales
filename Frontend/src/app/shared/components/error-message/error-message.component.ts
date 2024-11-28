import { Component, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'app-error-message',
  template: `
    <!-- Debug info -->
    <div style="display: none;">Error state: {{error}}</div>
    
    <!-- Actual error message -->
    <div class="status-message error" [style.display]="error ? 'block' : 'none'">
      {{ error }}
    </div>
  `,
  styles: [`
    .status-message {
      padding: 16px;
      border-radius: 8px;
      text-align: center;
      margin: 16px 0;
      font-weight: 500;
    }
    
    .error {
      background-color: #fff5f5;
      color: #e53e3e;
      border: 1px solid #feb2b2;
    }
  `]
})
export class ErrorMessageComponent implements OnChanges {
  @Input() error: string | null = null;

  ngOnChanges() {
    console.log('ErrorMessage received:', this.error); // Debug log
  }
} 