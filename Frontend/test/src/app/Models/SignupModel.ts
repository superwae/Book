export interface register {
    name: string;
    password: string;
    confirmNewPassword: string;
    email: string;
    confirmNewEmail: string;
    userName: string;
    profilePictureUrl?: string; // Optional property
    dthDate: string;
    city?: string; // Optional property
    about?: string; // Optional property
  }