import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

interface Contributor {
  id: number;
  name: string;
  phoneNumber: string | null;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  contributors: Contributor[] = [];
  isLoading = false;
  error: string | null = null;
  showContributors = false;

  constructor(private http: HttpClient) { }

  fetchContributors(): void {
    this.isLoading = true;
    this.showContributors = false;
    this.http.get<{ contributors: Contributor[] }>("https://localhost:57679/Contributors")
      .subscribe({
        next: (response) => {
          // Enhanced null and data handling
          this.contributors = response.contributors.map(contributor => ({
            ...contributor,
            name: contributor.name || 'Unknown',
            phoneNumber: contributor.phoneNumber
              ? this.formatPhoneNumber(contributor.phoneNumber)
              : 'No Phone Number'
          }));
          this.isLoading = false;
          this.showContributors = true;
        },
        error: (error) => {
          console.error('Error fetching contributors:', error);
          this.error = 'Failed to load contributors';
          this.isLoading = false;
          this.showContributors = false;
        }
      });
  }

  // Format phone number for better readability
  private formatPhoneNumber(phoneNumber: string): string {
    // Remove non-digit characters
    const cleaned = phoneNumber.replace(/\D/g, '');

    // Check if the number is valid
    if (cleaned.length === 10) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6)}`;
    }
    return phoneNumber;
  }
}
