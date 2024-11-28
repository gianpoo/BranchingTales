import { FormBuilder, Validators } from "@angular/forms";

/** Handles the contributor form with reactive forms */
export class ContributorFormComponent {
  form = this.fb.group({
    name: ['', Validators.required],
    phoneNumber: ['']
  });

  constructor(private fb: FormBuilder) {}
} 