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
  private currentChatId: number = 1;
  iterationLimit: number = 3;

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

  private async getRandomResponse(): Promise<string[]> {
    try {
      const response = await firstValueFrom(this.promptService.getRandomResponse());
      return response.options;
    } catch (error) {
      console.error('Error getting random response:', error);
      return ['I am thinking about what happens next...'];
    }
  }

  private async getAllPrompts(): Promise<string> {
    try {
      const response = await firstValueFrom(this.promptService.getChatPrompts(this.currentChatId));
      if (!response.prompts?.length) {
        return 'No messages found in this chat.';
      }
      return 'Here are the messages in this chat:\n\n' + 
             response.prompts
               .map((p, i) => `${i + 1}. ${p.text}`)
               .join('\n');
    } catch (error) {
      console.error('Error getting chat prompts:', error);
      return 'Failed to retrieve chat messages.';
    }
  }

  private async addUserMessage(text: string, id: number): Promise<void> {
    this.messages.push({
      id,
      text,
      isUser: true
    });

    // Get response based on command
    if (text.trim().toLowerCase() === "getallprompts") {
      const allPromptsResponse = await this.getAllPrompts();
      this.messages.push({
        id: -this.messages.length,
        text: allPromptsResponse,
        isUser: false
      });
    } else {
      // Get array of responses and add each as separate message
      const aiResponses = await this.getRandomResponse();
      aiResponses.forEach(response => {
        this.messages.push({
          id: -this.messages.length,
          text: response,
          isUser: false
        });
      });
    }
  }

  createPrompt(): void {
    if (!this.newPromptText.trim()) {
      this.error = 'Please enter your message';
      return;
    }

    if (!this.hasStarted && (this.iterationLimit < 2 || this.iterationLimit > 5)) {
      this.error = 'Please select a valid story length (2-5)';
      return;
    }

    const userMessage = this.newPromptText.trim();
    this.isLoading = true;
    this.error = null;

    if (!this.hasStarted) {
      // Create new chat - this will clear any existing chat
      this.promptService.create(userMessage, this.iterationLimit).subscribe({
        next: async (result) => {
          console.log('New chat created:', result);
          this.messages = []; // Clear existing messages
          this.currentChatId = 1;
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
