import { Component } from '@angular/core';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'StoryTellerFrontend';

  openPopupV: boolean = false;
  // If Want to use Function for popup
  openPopup(): void {
    this.openPopupV = !this.openPopupV;
  }
}

