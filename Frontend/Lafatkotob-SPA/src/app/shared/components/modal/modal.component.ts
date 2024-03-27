import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BookService } from '../../../Book/Service/BookService';
import { jwtDecode } from 'jwt-decode';
import { ModaleService } from '../../Service/ModalService/modal.service';
import { MyTokenPayload } from '../../Models/MyTokenPayload';
import { AppUsereService } from '../../../Auth/services/appUserService/app-user.service';
import { AppUserModel } from '../../../Auth/Models/AppUserModel';
import { HistoryService } from '../../Service/HistoryService/history.service';
import { SetUserHistoryModel } from '../../../Auth/Models/SetUserHistoryModel';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {
  @Input() title: string = '';
  @Input() show: boolean = false;
  @Output() closeEvent = new EventEmitter<void>();
  
  selectedImage: File | null = null;
  selectedImageUrl: string | null = null;
  bookForm!: FormGroup;
  private readonly NAME_IDENTIFIER_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

  constructor(
    private modalService: ModaleService,
     private fb: FormBuilder, 
     private bookService: BookService,
     private userService:AppUsereService,
     private historyService: HistoryService
     
     ) {}

  ngOnInit() {
    const userInfo = this.getUserInfoFromToken();
    if (userInfo && userInfo[this.NAME_IDENTIFIER_CLAIM]) {
      this.userService.getUserById(userInfo[this.NAME_IDENTIFIER_CLAIM]).subscribe({
                next: (user: AppUserModel) => {
          this.initializeForm(user.historyId);
        },
        error: (error) => {
          console.error('Error fetching user info', error);
          this.initializeForm();
        }
      });
    } else {
      this.initializeForm();
    }
  }
  initializeForm(historyId?: number) {
    this.bookForm = this.fb.group({
      Title: ['', Validators.required],
      Author: ['', Validators.required],
      Description: [''],
      CoverImage: [null],
      UserId: [this.getUserInfoFromToken()?.[this.NAME_IDENTIFIER_CLAIM], Validators.required],
      HistoryId: [historyId], 
      PublicationDate: [],
      ISBN: ['', [Validators.required, Validators.pattern(/^\d{13}$/)]],
      PageCount: [null],
      Condition: ['', Validators.required],
      Status: ['Available', Validators.required],
      Type: ['', Validators.required],
      PartnerUserId: [this.getUserInfoFromToken()?.[this.NAME_IDENTIFIER_CLAIM]],
      Language: ['', Validators.required],
      AddedDate: [new Date().toISOString()]
    });
  }

  getUserInfoFromToken(): MyTokenPayload | undefined {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: MyTokenPayload = jwtDecode<MyTokenPayload>(token);
      return decodedToken;
    }
    return undefined;
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length) {
      const file = files[0];
      this.selectedImage = file;

      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.selectedImageUrl = e.target?.result as string;

      };
      reader.readAsDataURL(file);
    }
  }

  async register() {
    if (this.bookForm.valid && this.selectedImage) {
      if (this.bookForm.value.HistoryId == null) {
        // Call HistoryService to create a new history
  
        this.historyService.postHistory(this.getUserInfoFromToken()?.[this.NAME_IDENTIFIER_CLAIM]!).subscribe({
          next: (history) => {
            const userId = this.getUserInfoFromToken()?.[this.NAME_IDENTIFIER_CLAIM];
            if (userId && history.historyId ) {
              const data: SetUserHistoryModel = {
                UserId: userId,
                HistoryId: history.historyId
            }; 
              this.userService.updateUserHistoryId(data).subscribe({
                next: () => {
                  this.bookForm.patchValue({ historyId: history.historyId });
                  console.log('User updated with new history ID', history.historyId);
                    this.proceedWithRegistration();
                },
                error: (error) => {
                  console.error('Error updating user with new history ID', error);
                  // Handle error, maybe notify the user
                }
              });
            } else {
              console.error('History ID not found in response or User ID not found');
              // Handle error, maybe notify the user
            }
          },
          error: (error) => {
            console.error('Error creating history', error);
            // Handle error, maybe notify the user
          }
        });
      } else {
        // If historyId is not null, proceed with the registration
        this.proceedWithRegistration();
      }
    } else {
      console.error('Form is not valid');
      // Handle form errors, maybe display error messages to user
    }
  }
  
  
  async  proceedWithRegistration() {
    const formData = new FormData();
  
  
    // Append all form fields to formData
    Object.keys(this.bookForm.value).forEach(key => {
      if (key !== 'coverImage') { 
        formData.append(key, this.bookForm.value[key]);
      }
    });
  
    // Append the file to formData if selectedImage is not null
    if (this.selectedImage) {
      formData.append('imageFile', this.selectedImage, this.selectedImage.name);
    }
    console.log("Final form values before submission:", this.bookForm.value);
    // Call the service method to submit the formData
    this.bookService.registerBook(formData).subscribe({
      next: (response) => {
        console.log('Book registered successfully', response);
        this.resetForm();
      },
      error: (error) => {
        console.error('Error registering book', error);
        // Handle error, you may want to log specific error details here as well
      }
    });
  }
  


  close() {
    this.show = false;
    this.modalService.setShowModal(false);
    this.closeEvent.emit();
    this.resetForm();
  }
  removeSelectedImage() {
    this.selectedImage = null;
    this.selectedImageUrl = null;
  }
  resetForm() {
    this.bookForm.reset({
      Title: '',
      Author: '',
      Description: '',
      CoverImage: null,
      PublicationDate: new Date().toISOString(),
      ISBN: '',
      PageCount: null,
      Status: 'Available',
    });
    this.selectedImage = null;
    this.selectedImageUrl = null;
    
  }
  
}


