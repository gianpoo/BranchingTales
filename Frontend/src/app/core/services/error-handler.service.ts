import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiError } from '@core/types/api.types';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  handleError(error: HttpErrorResponse): string {
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      return error.error.message;
    }
    
    // Server-side error
    const apiError: ApiError = {
      message: error.error?.message || 'An unknown error occurred',
      statusCode: error.status
    };

    switch (apiError.statusCode) {
      case 404:
        return 'Resource not found';
      case 400:
        return apiError.message;
      case 500:
        return 'Internal server error';
      default:
        return 'An unexpected error occurred';
    }
  }
} 