import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent {
  constructor(private router: Router){

  }
  ngOnInit(): void {
    localStorage.removeItem("user");
    this.router.navigateByUrl(`/login`);
  }
}
