export interface IdentityClaims {
  aud: string;
  azp: string;
  email: string;
  email_verified: boolean;
  exp: number;
  iat: number;
  iss: string;
  name: string;
  nonce: string;
  oi_au_id: string;
  oi_tkn_id: string;
  role: string[];
  sub: string;
}
