import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { ContributorListComponent } from './features/contributors/contributor-list/contributor-list.component';
//import { PromptSearchComponent } from './features/prompts/prompt-search/prompt-search.component';

@NgModule({
  declarations: [
    AppComponent,
    ContributorListComponent
    //PromptSearchComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
