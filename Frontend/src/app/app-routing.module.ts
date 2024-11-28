import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContributorListComponent } from './components/contributors/contributor-list/contributor-list.component';
import { PromptSearchComponent } from './components/prompts/prompt-search/prompt-search.component';

const routes: Routes = [
  { path: 'contributors', component: ContributorListComponent },
  { path: 'prompts', component: PromptSearchComponent },
  { path: '', redirectTo: '/contributors', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
