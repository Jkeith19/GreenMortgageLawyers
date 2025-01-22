import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { catchError } from 'rxjs';
import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LVRResult } from '../models/lvr.model';

@Component({
  selector: 'app-lvr-calculator',
  standalone: true,
  templateUrl: './lvr-calculator.component.html',
  styleUrls: ['./lvr-calculator.component.css'],
  imports: [FormsModule, HttpClientModule, CommonModule],
})
export class LvrCalculatorComponent {
  loanAmount: number = 0;
  propertyValue: number = 0;
  lvr: number | null = null;
  error: string = '';

  constructor(private http: HttpClient) {}

  // Handle form submission
  onSubmit() {
    this.error = '';

    if (this.loanAmount <= 0 || this.propertyValue <= 0) {
      this.error = 'Please enter valid numbers for both Loan Amount and Property Value.';
      return;
    }

    // Request data
    const requestData = {
      LoanAmount: this.loanAmount,
      PropertyValue: this.propertyValue,
    };

    // API call
    this.http
      .post<LVRResult>('https://localhost:7104/api/LVR', requestData)
      .pipe(
        catchError((err) => {
          this.error = 'Failed to calculate LVR. Please try again.';
          this.lvr = null;
          return of(null);
        })
      )
      .subscribe((response) => {
        if (response) {
          if (response.statusCode === 201) {
            const lvrValue = response.lvr; 
            this.lvr = lvrValue;
          } 
          else {
            this.error = response.message || 'An unexpected error occurred.';
            this.lvr = null;
          }
        } 
        else {
          console.log(response);
          this.error = 'No response received from the server.';
          this.lvr = null;
        }
      });
  }
}