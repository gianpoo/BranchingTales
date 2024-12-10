import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { PromptService } from './core/services/prompt.service';
import { HttpErrorResponse } from '@angular/common/http';

interface ChatMessage {
  id?: number;
  text: string;
  isUser?: boolean;
  isOption?: boolean;
  isSelected?: boolean;
  disabled?: boolean;
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
  messages: ChatMessage[] = [];
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
      next: (response) => {
        this.isChatMode = true;
        this.messages = [{ text: this.storyPrompt, isUser: true }];
        this.getRandomResponse();
        this.storyPrompt = '';
        setTimeout(() => {
          this.chatInput?.nativeElement?.focus();
        }, 0);
      },
      error: (err: HttpErrorResponse) => {
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
      this.promptService.getChatPrompts(1).subscribe({
        next: (response: { prompts: Array<{ id: number, text: string }> }) => {
          if (!response.prompts?.length) {
            this.messages.push({ text: 'No prompts found.', isUser: false });
          } else {
            const allPromptsMessage = response.prompts
              .map((p: { id: number, text: string }, index: number) => 
                `${index + 1}. ${p.text}`
              )
              .join('\n\n');
            this.messages.push({ 
              text: allPromptsMessage, 
              isUser: false
            });
          }
          this.storyPrompt = '';
        },
        error: (err: Error) => {
          console.error('Error getting all prompts:', err);
          this.messages.push({ text: 'Failed to retrieve prompts.', isUser: false });
        }
      });
      return;
    }

    this.messages.push({ text: this.storyPrompt, isUser: true });
    this.promptService.addPrompt(this.storyPrompt).subscribe({
      next: (response) => {
        console.log('Prompt added:', response);
        this.getRandomResponse();
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error adding prompt:', err);
        this.messages.push({ text: 'Failed to save your message.', isUser: false });
      }
    });

    this.storyPrompt = '';
  }

  private getRandomResponse() {
    this.promptService.getRandomResponse().subscribe({
      next: (response) => {
        if (response.options && response.options.length > 0) {
          // Clear any previous options first
          this.messages = this.messages.filter(m => !m.isOption);
          
          // Add the question
          this.messages.push({ 
            text: "How would you like the story to continue?", 
            isUser: false 
          });
          
          // Add new options
          response.options.forEach(option => {
            this.messages.push({ 
              text: option, 
              isUser: false,
              isOption: true,
              isSelected: false,
              disabled: false
            });
          });
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error getting response:', err);
      }
    });
  }

  selectOption(selectedMessage: ChatMessage) {
    if (!selectedMessage.isOption || selectedMessage.disabled) return;

    // Disable all options
    this.messages
      .filter(m => m.isOption)
      .forEach(m => {
        m.disabled = true;
        m.isSelected = m === selectedMessage;
      });

    this.storyPrompt = selectedMessage.text;
    this.onSend();
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
}
