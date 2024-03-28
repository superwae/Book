import { Component, Input, OnInit } from '@angular/core';
import { MessageComponent } from '../message/message.component';
import { CommonModule } from '@angular/common';
import { MessageModel } from '../../models/Message';
import { UserInMessages } from '../../models/UserInMessages';
import { AppUsereService } from '../../../Auth/services/appUserService/app-user.service';
import { ConversationsUserModel } from '../../models/ConversationUser';

@Component({
  selector: 'app-conversation',
  standalone: true,
  imports: [MessageComponent, CommonModule],
  templateUrl: './conversation.component.html',
  styleUrl: './conversation.component.css'
})
export class ConversationComponent implements OnInit {
  @Input() conversationUser: ConversationsUserModel | null = null;
  currentUserInfo: UserInMessages | null = null;
  otherUserInfo: UserInMessages | null = null;
  messages: MessageModel[] = [
    {
      id: 1,
      conversationId: 1,
      senderUserId: '06eb3e92-d077-47d6-80db-13b681e2f1a3',
      receiverUserId: '55b6e49c-5021-4685-b1ca-cf0e65f48909',
      messageText: 'Hi from the user!',
      dateSent: new Date(),
      isReceived: true,
      isRead: false,
      isDeletedBySender: false,
      isDeletedByReceiver: false,
    },
    {
      id: 2,
      conversationId: 1,
      senderUserId: '55b6e49c-5021-4685-b1ca-cf0e65f48909',
      receiverUserId: '06eb3e92-d077-47d6-80db-13b681e2f1a3',
      messageText: 'Hello back to you!',
      dateSent: new Date(),
      isReceived: true,
      isRead: true,
      isDeletedBySender: false,
      isDeletedByReceiver: false,
    },
    {
      id: 3,
      conversationId: 1,
      senderUserId: '55b6e49c-5021-4685-b1ca-cf0e65f48909',
      receiverUserId: '06eb3e92-d077-47d6-80db-13b681e2f1a3',
      messageText: 'How are you?',
      dateSent: new Date(),
      isReceived: true,
      isRead: false,
      isDeletedBySender: false,
      isDeletedByReceiver: false,
    }
  ];
  constructor(private AppUsereService: AppUsereService) { }
  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadOtherUser();
  }


  private loadCurrentUser(): void {
    this.currentUserInfo = {
      userid: '',
      userName: '',
      profilePicture: ''
    };
    const toekn = localStorage.getItem('token');
    if (toekn) {
      this.currentUserInfo.userid = localStorage.getItem('userId') || '';
      this.currentUserInfo.userName = localStorage.getItem('userName') || 'Unknown User';
      this.currentUserInfo.profilePicture = localStorage.getItem('profilePicture') || 'path/to/default/image';
    }

  }

  private loadOtherUser(): void {
      this.AppUsereService.getUserById(this.conversationUser?.userId!).subscribe(user => {
        this.otherUserInfo = {
          userid: user.id,
          userName: user.name, 
          profilePicture: user.profilePicture 
        };
      });
    }
  

  }
