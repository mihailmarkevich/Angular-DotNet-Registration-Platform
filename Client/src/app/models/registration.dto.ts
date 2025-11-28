export interface RegistrationRequestDto {
  companyName: string;
  industryId: number;
  firstName: string;
  lastName: string;
  userName: string;
  email?: string | null;
  password: string;
  passwordRepeat: string;
  acceptTerms: boolean;
  acceptPrivacy: boolean;
}

export interface RegistrationResponseDto {
  companyId: number;
  userId: number;
}
