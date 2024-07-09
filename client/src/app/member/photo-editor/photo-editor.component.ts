import { Component, inject, input, OnInit, output } from '@angular/core';
import { Memeber } from '../../models/Member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment.development';
import { MembersService } from '../../_services/members.service';
import { Photo } from '../../models/Photo';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgStyle, NgClass, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {

  private accountService = inject(AccountService);
  uploader?: FileUploader;
  member = input.required<Memeber>();
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  memberChange = output<Memeber>();
  private memberService= inject(MembersService);


  ngOnInit(): void {
    this.intializeUploader();
  }


  fileOverBase(e : any){
    this.hasBaseDropZoneOver = e;
  }

  deletePhoto(photo: Photo){
    this.memberService.deletePhoto(photo).subscribe({
      next: _ =>{
        const updatedMember ={...this.member()};
        updatedMember.photos = updatedMember.photos.filter(x => x.id !== photo.id);
        this.memberChange.emit(updatedMember);

      }
    })
  }
  setMainPhoto(photo : Photo){
    this.memberService.setMainPhoto(photo).subscribe({
      next: _ =>{
        const user = this.accountService.currentUser();
        if(user){
          user.photoUrl = photo.url;
          this.accountService.serCurrentUser(user);
        }
        const updatedMember = {...this.member()};
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          if(p.isMain)
            p.isMain = false;
          if(p.id === photo.id)
            p.isMain = true;
        })
        this.memberChange.emit(updatedMember);
      }
    });
  }

  intializeUploader(){
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.tokenKey,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    })
    this.uploader.onAfterAddingFile = (file)=>{
      file.withCredentials = false
    } 
    this.uploader.onSuccessItem = (item, response, status, headers) =>{
      const photo = JSON.parse(response);
      const updatedMember = {...this.member()}
      updatedMember.photos.push(photo);
      this.memberChange.emit(updatedMember);

    }
  }

}
