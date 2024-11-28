export const API_ENDPOINTS = {
  CONTRIBUTORS: 'Contributors',
  PROMPTS: 'Prompts'
} as const;

export const ERROR_MESSAGES = {
  SEARCH_LENGTH: 'Search term must be at least 3 characters long',
  LOAD_FAILED: 'Failed to load data',
  SEARCH_FAILED: 'Failed to search',
  UPDATE_FAILED: 'Failed to update',
  DELETE_FAILED: 'Failed to delete',
  REQUIRED_FIELD: 'This field is required'
} as const; 