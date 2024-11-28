import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { ContributorListComponent } from './contributor-list/contributor-list.component';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    ContributorListComponent
  ],
  exports: [
    ContributorListComponent
  ]
})
export class ContributorsModule { }