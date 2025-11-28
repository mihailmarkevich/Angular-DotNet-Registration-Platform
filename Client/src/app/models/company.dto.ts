/**
 * Company suggestion model used for autocomplete.
 * Backend should expose an endpoint that returns data in this shape.
 */
export interface CompanySuggestionDto {
  id: number;
  name: string;
  industryId: number;
}
