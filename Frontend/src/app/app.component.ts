import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { PromptService } from './core/services/prompt.service';
import { HttpErrorResponse } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

interface ChatMessage {
  id?: number;
  text: string;
  isUser?: boolean;
  isOption?: boolean;
  isSelected?: boolean;
  disabled?: boolean;
}

// Add this new interface for recap messages
interface RecapMessage extends ChatMessage {
  isRecap?: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewChecked {
  title = 'Branching Tales';
  isDarkMode = true;
  storyPrompt = '';
  isLoading = false;
  error = '';
  isChatMode = false;
  messages: RecapMessage[] = [];
  @ViewChild('chatInput') chatInput!: ElementRef;
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  iterationLimit: number = 3;

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

    this.promptService.create(this.storyPrompt, this.iterationLimit).subscribe({
      next: () => {
        this.isChatMode = true;
        this.messages = [{ text: this.storyPrompt, isUser: true }];
        this.getOptions();
        this.storyPrompt = '';
        setTimeout(() => {
          this.chatInput?.nativeElement?.focus();
        }, 0);
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error starting story:', err);
        this.error = 'Failed to start the story. Please try again.';
        this.isLoading = false;
      }
    });
  }

  onSend() {
    if (!this.storyPrompt.trim()) {
      this.error = 'Please enter your message';
      return;
    }

    this.isLoading = true;
    this.error = '';

    this.promptService.addPrompt(this.storyPrompt).subscribe({
      next: () => {
        // Add user message
        this.messages.push({
          text: this.storyPrompt,
          isUser: true
        });
        this.storyPrompt = '';

        // Get AI options
        this.getOptions();
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error sending message:', err);
        this.isLoading = false;
        this.error = 'Failed to send message';
      }
    });
  }

  private getOptions() {
    this.promptService.getOptions().subscribe({
      next: (response) => {
        // Add a recap message first
        const recap = this.generateRecap();
        this.messages.push({
          text: `Story so far:\n${recap}`,
          isUser: false,
          isRecap: true
        });

        // Then add the options
        response.options.forEach(option => {
          this.messages.push({
            text: option,
            isUser: false,
            isOption: true,
            isSelected: false,
            disabled: false
          });
        });
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error getting options:', err);
        this.isLoading = false;
        this.error = 'Failed to get story options';
      }
    });
  }

  selectOption(selectedMessage: ChatMessage) {
    if (!selectedMessage.isOption || selectedMessage.disabled) return;

    // Remove all options from messages array
    this.messages = this.messages.filter(m => !m.isOption);

    this.isLoading = true;
    this.error = '';

    // Send the selected option
    this.promptService.addPrompt(selectedMessage.text).subscribe({
      next: () => {
        // Add user's choice as a message
        this.messages.push({
          text: selectedMessage.text,
          isUser: true
        });

        // Get new options
        this.getOptions();
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error sending option:', err);
        this.error = 'Failed to send your choice';
        this.isLoading = false;
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

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      const element = this.messagesContainer.nativeElement;
      element.scrollTop = element.scrollHeight;
    } catch(err) { }
  }

  // New method to find the first option index
  findFirstOptionIndex(): number {
    return this.messages.findIndex(m => m.isOption);
  }

  isFirstOption(message: ChatMessage, index: number): boolean {
    return (message.isOption ?? false) && index === this.findFirstOptionIndex();
  }

  getOptionsSlice(index: number): ChatMessage[] {
    return this.messages.slice(index, index + 3);
  }

  handleOptionClick(option: ChatMessage): void {
    if (!option.disabled) {
      this.selectOption(option);
    }
  }

  getCursorStyle(option: ChatMessage): string {
    return option.disabled ? 'default' : 'pointer';
  }

  // Add this new method to generate recap
  private generateRecap(): string {
    return this.messages
      .filter(m => m.isUser && !m.isOption)
      .map(m => m.text)
      .join('\n');
  }
}
