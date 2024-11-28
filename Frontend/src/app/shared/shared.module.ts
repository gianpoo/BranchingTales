import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ErrorMessageComponent } from './components/error-message/error-message.component';
import { LoadingIndicatorComponent } from './components/loading-indicator/loading-indicator.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ErrorMessageComponent,
    LoadingIndicatorComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ErrorMessageComponent,
    LoadingIndicatorComponent
  ]
})
export class SharedModule { } 