import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Memeber } from '../models/Member';
import { environment } from '../../environments/environment.development';
import { of, tap } from 'rxjs';
import { Photo } from '../models/Photo';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Memeber[]>([]);

  getMembers(){
    return this.http.get<Memeber[]>(this.baseUrl+'user').subscribe({
      next: members => this.members.set(members)
    });
  }

  getMember(username: string){
    const member = this.members().find(x=> x.userName === username);
    if(member !== undefined) {
      return of(member)
    }
    
    return this.http.get<Memeber>(this.baseUrl+'user/' + username);
  }

  updateMember(member: Memeber){
    return this.http.put(this.baseUrl+'user', member).pipe(
      tap(()=>{
        this.members.update(members => members
          .map(m => m.userName === member.userName ? member : m))
      })
    );
  }
  setMainPhoto(photo : Photo){
    return this.http.put(this.baseUrl + 'user/set-main-photo/'+ photo.id , {} ).pipe(
      tap(()=>{
        this.members.update(members => members.map(m =>{
          if (m.photos.includes(photo))
            m.photoUrl = photo.url;
          return m;
        }))
      })
    )
  }
  deletePhoto(photo : Photo){
    return this.http.delete(this.baseUrl + 'user/delete-photo/' + photo.id).pipe(
      tap(()=>{
        this.members.update(members => members.map(m=>{
          if(m.photos.includes(photo)){
            m.photos = m.photos.filter(x => x.id !== photo.id)
          }
          return m
        }))
      })
    );
  }
}
