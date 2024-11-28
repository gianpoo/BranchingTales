import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ContributorService } from '../../../core/services/contributor.service';
import { Contributor } from '../../../core/models/contributor.interface';
import { ERROR_MESSAGES } from '../../../core/constants/api.constants';

@Component({
  selector: 'app-contributor-list',
  templateUrl: './contributor-list.component.html'
})
export class ContributorListComponent implements OnInit {
  contributors: Contributor[] = [];
  isLoading = false;
  error: string | null = null;
  searchTerm = '';
  newContributorName = '';
  newContributorPhone: string | null = null;
  updateContributorId: number | null = null;
  updateContributorName = '';
  updateContributorPhone: string | null = null;
  showTable = false;
  searchType: 'name' | 'id' = 'name';

  constructor(private contributorService: ContributorService) { }

  ngOnInit() {
    console.log('Initial error state:', this.error);
  }

  fetchContributors(): void {
    this.isLoading = true;
    this.error = null;
    this.contributorService.getAll().subscribe({
      next: (response: { contributors: Contributor[] }) => {
        this.contributors = response.contributors;
        this.showTable = true;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error fetching contributors:', error);
        this.error = 'Failed to load contributors';
        this.isLoading = false;
      }
    });
  }

  searchContributors(): void {
    if (this.searchType === 'name' && this.searchTerm.length < 3) {
      this.error = 'Search term must be at least 3 characters long';
      return;
    }

    if (this.searchType === 'id' && isNaN(Number(this.searchTerm))) {
      this.error = 'Please enter a valid ID number';
      return;
    }

    this.isLoading = true;
    this.error = null;

    if (this.searchType === 'name') {
      this.contributorService.search(this.searchTerm).subscribe({
        next: (contributors) => {
          this.contributors = contributors;
          this.isLoading = false;
        },
        error: this.handleError.bind(this)
      });
    } else {
      this.contributorService.getById(Number(this.searchTerm)).subscribe({
        next: (contributor) => {
          this.contributors = contributor ? [contributor] : [];
          this.isLoading = false;
        },
        error: this.handleError.bind(this)
      });
    }
  }

  private handleError(error: HttpErrorResponse): void {
    console.error('Error:', error);
    this.error = error.status === 404 ? 'No contributors found' : 'Failed to search contributors';
    this.isLoading = false;
    this.contributors = [];
  }

  addContributor(): void {
    if (!this.newContributorName) {
      this.error = 'Contributor name is required';
      return;
    }

    this.isLoading = true;
    this.error = null;
    this.contributorService.create({ 
      name: this.newContributorName, 
      phoneNumber: this.newContributorPhone 
    }).subscribe({
      next: () => {
        this.newContributorName = '';
        this.newContributorPhone = null;
        this.fetchContributors();
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error adding contributor:', error);
        this.error = 'Failed to add contributor';
        this.isLoading = false;
      }
    });
  }

  startUpdate(contributor: Contributor): void {
    this.updateContributorId = contributor.id;
    this.updateContributorName = contributor.name;
    this.updateContributorPhone = contributor.phoneNumber;
  }

  updateContributor(): void {
    if (!this.updateContributorId || !this.updateContributorName) {
      this.error = 'Contributor ID and name are required';
      return;
    }

    this.isLoading = true;
    this.error = null;
    this.contributorService.update({
      id: this.updateContributorId,
      name: this.updateContributorName,
      phoneNumber: this.updateContributorPhone
    }).subscribe({
      next: () => {
        this.updateContributorId = null;
        this.updateContributorName = '';
        this.updateContributorPhone = null;
        this.fetchContributors();
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error updating contributor:', error);
        this.error = 'Failed to update contributor';
        this.isLoading = false;
      }
    });
  }

  deleteContributor(id: number): void {
    this.isLoading = true;
    this.error = null;
    this.contributorService.delete(id).subscribe({
      next: () => {
        this.fetchContributors();
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error deleting contributor:', error);
        this.error = 'Failed to delete contributor';
        this.isLoading = false;
      }
    });
  }
}