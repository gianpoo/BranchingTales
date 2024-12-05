import { Component, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { PromptService } from '../../../core/services/prompt.service';
import { firstValueFrom } from 'rxjs';

interface ChatMessage {
  id: number;
  text: string;
  isUser: boolean;
}

@Component({
  selector: 'app-prompt-search',
  templateUrl: './prompt-search.component.html'
})
export class PromptSearchComponent implements AfterViewChecked {
  @ViewChild('chatMessages') private messagesContainer!: ElementRef;
  
  newPromptText = '';
  hasStarted = false;
  isLoading = false;
  error: string | null = null;
  messages: ChatMessage[] = [];

  constructor(private promptService: PromptService) {}

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop = 
        this.messagesContainer.nativeElement.scrollHeight;
    } catch(err) {}
  }

  private async getRandomResponse(): Promise<string> {
    try {
      const response = await firstValueFrom(this.promptService.getRandomResponse());
      return response.options[0]; // Use the first option
    } catch (error) {
      console.error('Error getting random response:', error);
      return 'I am thinking about what happens next...';
    }
  }

  private async getAllPrompts(): Promise<string> {
    try {
      const response = await firstValueFrom(this.promptService.getAllPrompts());
      if (!response.prompts?.length) {
        return 'No messages found in history.';
      }
      return 'Here are your previous messages:\n\n' + 
             response.prompts
               .map((p, i) => `${i + 1}. ${p.text}`)
               .join('\n');
    } catch (error) {
      console.error('Error getting all prompts:', error);
      return 'Failed to retrieve message history.';
    }
  }

  private async addUserMessage(text: string, id: number): Promise<void> {
    this.messages.push({
      id,
      text,
      isUser: true
    });

    // Get response based on command
    const aiResponse = text.trim().toLowerCase() === "getallprompts" 
      ? await this.getAllPrompts()
      : await this.getRandomResponse();

    this.messages.push({
      id: -this.messages.length,
      text: aiResponse,
      isUser: false
    });
  }

  createPrompt(): void {
    if (!this.newPromptText.trim()) {
      this.error = 'Please enter your message';
      return;
    }

    const userMessage = this.newPromptText.trim();
    this.isLoading = true;
    this.error = null;

    if (!this.hasStarted) {
      // Create new chat
      this.promptService.create(userMessage).subscribe({
        next: async (result) => {
          console.log('Chat created:', result);
          await this.addUserMessage(userMessage, 1);
          this.newPromptText = '';
          this.isLoading = false;
          this.hasStarted = true;
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error creating chat:', error);
          this.error = 'Failed to send message';
          this.isLoading = false;
        }
      });
    } else {
      // Add prompt to existing chat
      this.promptService.addPrompt(userMessage).subscribe({
        next: async (result) => {
          console.log('Prompt added:', result);
          await this.addUserMessage(userMessage, this.messages.length + 1);
          this.newPromptText = '';
          this.isLoading = false;
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error adding prompt:', error);
          this.error = 'Failed to send message';
          this.isLoading = false;
        }
      });
    }
  }
} 
