import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Memeber } from '../models/Member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private hhtp = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getMembers(){
    return this.hhtp.get<Memeber[]>(this.baseUrl+'user');
  }

  getMember(username: string){
    return this.hhtp.get<Memeber>(this.baseUrl+'user/' + username);
  }

}
