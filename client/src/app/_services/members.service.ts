import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Memeber } from '../models/Member';
import { environment } from '../../environments/environment.development';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private hhtp = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Memeber[]>([]);

  getMembers(){
    return this.hhtp.get<Memeber[]>(this.baseUrl+'user').subscribe({
      next: members => this.members.set(members)
    });
  }

  getMember(username: string){
    const member = this.members().find(x=> x.userName === username);
    if(member !== undefined) {
      return of(member)
    }
    
    return this.hhtp.get<Memeber>(this.baseUrl+'user/' + username);
  }

  updateMember(member: Memeber){
    return this.hhtp.put(this.baseUrl+'user', member).pipe(
      tap(()=>{
        this.members.update(members => members
          .map(m => m.userName === member.userName ? member : m))
      })
    );
  }
}
