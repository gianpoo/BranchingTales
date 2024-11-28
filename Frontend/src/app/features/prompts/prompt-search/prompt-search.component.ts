import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { PromptService } from '../../../core/services/prompt.service';
import { Prompt } from '../../../core/models/prompt.interface';

@Component({
  selector: 'app-prompt-search',
  templateUrl: './prompt-search.component.html'
})
export class PromptSearchComponent {
  promptId: number | null = null;
  newPromptText = '';
  prompt: Prompt | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(private promptService: PromptService) {}

  searchPrompt(): void {
    if (!this.promptId) {
      this.error = 'Please enter a prompt ID';
      return;
    }

    this.isLoading = true;
    this.error = null;
    this.prompt = null;

    this.promptService.getById(this.promptId).subscribe({
      next: (result) => {
        this.prompt = result;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error fetching prompt:', error);
        this.error = error.status === 404 ? 'Prompt not found' : 'Failed to fetch prompt';
        this.isLoading = false;
      }
    });
  }

  createPrompt(): void {
    if (!this.newPromptText.trim()) {
      this.error = 'Please enter prompt text';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.promptService.create(this.newPromptText.trim()).subscribe({
      next: (result) => {
        this.prompt = result;
        this.newPromptText = '';
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error creating prompt:', error);
        this.error = 'Failed to create prompt';
        this.isLoading = false;
      }
    });
  }
} 