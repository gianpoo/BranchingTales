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
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  contributors: Contributor[] = [];
  isLoading = false;
  error: string | null = null;
  showContributors = false;
  searchTerm: string = '';
  searchType: 'name' | 'id' = 'name';

  // `newContributor` should only contain `name` and `phoneNumber`, without `id`
  newContributor: { name: string; phoneNumber: string | null } = { name: '', phoneNumber: null };

  // `editingContributor` is initially null and should have the complete Contributor object when editing
  editingContributor: Contributor | null = null;

  constructor(private http: HttpClient) { }

  // Fetch Contributors
  fetchContributors(): void {
    this.isLoading = true;
    this.error = null;
    this.http.get<{ contributors: Contributor[] }>('https://localhost:57679/Contributors').subscribe({
      next: (response) => {
        const data = response.contributors;  // Extract the contributors array
        if (Array.isArray(data) && data.length > 0) {
          this.contributors = data;
          this.showContributors = true;  // Show contributors if valid data is received
          console.log('Fetched Contributors:', this.contributors); // Debugging log
        } else {
          this.error = 'No contributors found.';
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error:', err);  // Log error details for debugging
        if (err.status) {
          this.error = `Failed to load contributors. Status: ${err.status}`;
        } else {
          this.error = 'An unknown error occurred.';
        }
        this.isLoading = false;
      },
    });
  }


  // Add Contributor
  addContributor(): void {
    const newContributorToSend = { ...this.newContributor };  // Don't include `id` here
    this.http.post<Contributor>('https://localhost:57679/Contributors', newContributorToSend, {
      headers: {
        'Content-Type': 'application/json'
      }
    }).subscribe({
      next: () => {
        this.fetchContributors();
        this.editingContributor = null; // Reset the form
      },
      error: (err) => {
        console.error('Error adding contributor', err); // Log the error to the console
        this.error = 'Failed to add contributor.';
      },
    });
  }


  // Format phone number (for example, US format)
  formatPhoneNumber(phoneNumber: string | null): string {
    if (!phoneNumber) return 'No phone number available';
    const phone = phoneNumber.replace(/\D/g, ''); // Remove non-numeric characters
    if (phone.length === 10) {
      return `(${phone.substring(0, 3)}) ${phone.substring(3, 6)}-${phone.substring(6)}`;
    }
    return phoneNumber;  // Return original if not a 10-digit number
  }

  // Set Contributor for Update (populate form with the contributor data to edit)
  setUpdateForm(contributor: Contributor): void {
    this.editingContributor = { ...contributor }; // This ensures we have the full contributor object (including `id`)
  }

  // Update Contributor
  updateContributor(): void {
    if (!this.editingContributor) return;
    console.log('Updating contributor:', this.editingContributor); // Debugging log

    const contributorToUpdate = { ...this.editingContributor };

    this.http.put<Contributor>(`https://localhost:57679/Contributors/${contributorToUpdate.id}`, contributorToUpdate).subscribe({
      next: () => {
        console.log('Update successful, refetching contributors...');
        this.fetchContributors(); // This will refresh the table with the updated data
        this.editingContributor = null; // Reset the form
      },
      error: () => {
        this.error = 'Failed to update contributor.';
      }
    });
  }



  deleteContributor(id: number): void {
    this.http.delete(`https://localhost:57679/Contributors/${id}`).subscribe({
      next: () => {
        this.fetchContributors();  // Reload contributors after deleting
        this.editingContributor = null; // Reset the form
      },
      error: () => {
        this.error = 'Failed to delete contributor.';
      },
    });
  }

  searchContributors(): void {
    const trimmedSearch = this.searchTerm.trim();
    if (this.searchType === 'name' && trimmedSearch.length < 3) {
        this.error = 'Please enter at least 3 characters for name search';
        return;
    }
    if (!trimmedSearch) {
        this.error = 'Please enter a search term';
        return;
    }
    
    this.isLoading = true;
    this.error = null;
    
    let url = `https://localhost:57679/Contributors/search/${encodeURIComponent(trimmedSearch)}`;

    this.http.get<{ contributors: Contributor[] }>(url).subscribe({
        next: (response) => {
            if (Array.isArray(response)) {  // Backend returns array directly
                this.contributors = response;
                this.showContributors = true;
            } else {
                this.error = 'Invalid response format';
            }
            this.isLoading = false;
        },
        error: (err) => {
            console.error('Search error:', err);
            this.error = 'Failed to search contributors';
            this.isLoading = false;
        }
    });
  }
}
