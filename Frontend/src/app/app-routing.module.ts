import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PromptSearchComponent } from './components/prompts/prompt-search/prompt-search.component';

const routes: Routes = [
  { path: 'prompts', component: PromptSearchComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
