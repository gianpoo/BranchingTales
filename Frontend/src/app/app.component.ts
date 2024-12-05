import { Component, OnInit } from '@angular/core';
import { PromptService } from './core/services/prompt.service';

interface ChatMessage {
  id?: number;
  text: string;
  isUser?: boolean;
}

interface Prompt {
  id: number;
  text: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Branching Tales';
  isDarkMode = true;
  storyPrompt = '';
  isLoading = false;
  error = '';
  isChatMode = false;
  messages: ChatMessage[] = [];

  constructor(private promptService: PromptService) {}

  ngOnInit() {
    document.body.classList.add('dark-theme');
  }

  toggleTheme() {
    this.isDarkMode = !this.isDarkMode;
    document.body.classList.toggle('dark-theme');
  }

  onBegin() {
    if (!this.storyPrompt.trim()) {
      this.error = 'Please enter your story prompt';
      return;
    }

    this.isLoading = true;
    this.error = '';

    this.promptService.create(this.storyPrompt).subscribe({
      next: (response) => {
        this.isChatMode = true;
        this.messages = [{ text: this.storyPrompt, isUser: true }];
        this.getRandomResponse();
        this.storyPrompt = '';
      },
      error: (err) => {
        console.error('Error starting story:', err);
        this.error = 'Failed to start the story. Please try again.';
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onSend() {
    if (!this.storyPrompt.trim()) return;
    
    if (this.storyPrompt.toLowerCase() === 'get all prompts') {
      this.messages.push({ text: this.storyPrompt, isUser: true });
      this.promptService.getAllPrompts().subscribe({
        next: (response) => {
          if (!response.prompts?.length) {
            this.messages.push({ text: 'No prompts found.', isUser: false });
          } else {
            const allPromptsMessage = response.prompts.map(p => p.text).join('\n');
            this.messages.push({ text: allPromptsMessage, isUser: false });
          }
          this.storyPrompt = '';
        },
        error: (err) => {
          console.error('Error getting all prompts:', err);
          this.messages.push({ text: 'Failed to retrieve prompts.', isUser: false });
        }
      });
      return;
    }
    
    this.messages.push({ text: this.storyPrompt, isUser: true });
    this.getRandomResponse();
    this.storyPrompt = '';
  }

  private getRandomResponse() {
    this.promptService.getRandomResponse().subscribe({
      next: (response) => {
        if (response.options && response.options.length > 0) {
          this.messages.push({ text: response.options[0], isUser: false });
        }
      },
      error: (err) => {
        console.error('Error getting response:', err);
      }
    });
  }

  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      if (this.isChatMode) {
        this.onSend();
      } else {
        this.onBegin();
      }
    }
  }
}
