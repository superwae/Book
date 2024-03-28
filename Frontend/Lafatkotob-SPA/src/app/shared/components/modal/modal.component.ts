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
import { ModalGenreComponent } from '../modal-genre/modal-genre.component';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,ModalGenreComponent],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {
  @Input() title: string = '';
  @Input() show: boolean = false;
  @Output() closeEvent = new EventEmitter<void>();
  bookForm!: FormGroup;
  showGenreSelection: boolean = false;
  registrationData: any;
  showModalGenre: boolean = false;
  isLookingFor:boolean = false;
  private readonly NAME_IDENTIFIER_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

  constructor(
    private modalService: ModaleService,
     private fb: FormBuilder, 
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

  async register() {
    console.log(this.bookForm.value);
console.log(this.bookForm.valid);
console.log(this.bookForm.errors);
    if (this.bookForm.valid) {
     
      if (this.bookForm.value.HistoryId == null) {
  
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
                }
              });
            } else {
              console.error('History ID not found in response or User ID not found');
            }
          },
          error: (error) => {
            console.error('Error creating history', error);
          }
        });
      } else {
        this.proceedWithRegistration();
      }
    } else {
      console.error('Form is not valid');
    }
  }

  onTypeChange(event: any): void {
    this.isLookingFor = event.target.value === 'buy';
}
  
  async  proceedWithRegistration() {

  
  
    const formData = new FormData();
  
  
    Object.keys(this.bookForm.value).forEach(key => {
        formData.append(key, this.bookForm.value[key]);
    });
    this.registrationData = formData; 
    this.showGenreSelection = true;
  }
  

  handleProceedToGenreSelection(data: any) {
    this.registrationData = data;
    this.showModalGenre = true;
  }
  handleGenreModalClose() {
    this.showModalGenre = false;
  }
  close() {
    this.show = false;
    this.modalService.setShowModal(false);
    this.closeEvent.emit();
    this.resetForm();
  }
  closeEvent2():void {
    this.close();
  }
  onAddAnotherBook(): void {
    this.showGenreSelection = false;
    this.show = true;
    this.resetForm(); 
   
  }
  onClosePopup(): void {
    this.showGenreSelection = false;
    this.show = false; 
  this.modalService.setShowModal(false); 
  this.closeEvent.emit();
    
  }

  resetForm() {
    const userId = this.bookForm.get('UserId')?.value;
  const historyId = this.bookForm.get('HistoryId')?.value;
  const PartnerUserId= this.bookForm.get('PartnerUserId')?.value;
    this.bookForm.reset({
      Title: '',
      Author: '',
      Description: '',
      CoverImage: null,
      PublicationDate: '',
      ISBN: '',
      PageCount: '',
      Condition: '',
      Status: 'Available', // Assuming you want a default status
      Type: '',
      PartnerUserId: '',
      Language: '',
      AddedDate: new Date().toISOString(), // or any other default value
    });
    this.isLookingFor = false;
    this.bookForm.patchValue({
      UserId: userId,
      HistoryId: historyId,
      PartnerUserId: PartnerUserId

    });
  
}
}


