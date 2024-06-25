import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [RegisterComponent]
})
export class HomeComponent implements OnInit {
  registerMode =false;
  httpClient =inject(HttpClient);
  users : any;
    
  
  
  ngOnInit(): void {
    this.getUser();

  }
  getUser(){
    this.httpClient.get('https://localhost:5001/api/user').subscribe({
      next:response=> this.users=response,
      error:err=> console.log(err),
      complete:()=>console.log("request completed")
      
    })
  }
  registeToggle(){
    this.registerMode = !this.registerMode;
  }
  cancelRegisterMode(event : boolean){
    this.registerMode = event;
  }
}
