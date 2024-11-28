import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { PromptSearchComponent } from './prompt-search/prompt-search.component';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    PromptSearchComponent
  ],
  exports: [
    PromptSearchComponent
  ]
})
export class PromptsModule { } 