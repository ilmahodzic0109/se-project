import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockLoginService: jasmine.SpyObj<LoginService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockLoginService = jasmine.createSpyObj('LoginService', ['login']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [
        { provide: LoginService, useValue: mockLoginService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should login successfully and navigate to homepage', fakeAsync(() => {
    const mockResponse = { userId: '123', isAdmin: false, isLogged: true };

    component.loginData.email = 'test@example.com';
    component.loginData.password = 'password123';

    mockLoginService.login.and.returnValue(of(mockResponse));

    component.onLogin();

    expect(component.successMessage).toBe('Login successful! Welcome back.');
    expect(component.errorMessage).toBeNull();

    expect(localStorage.getItem('userId')).toBe('123');
    expect(localStorage.getItem('isAdmin')).toBe('false');
    expect(localStorage.getItem('isLogged')).toBe('true');
    tick(3000);

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/homepage']);
  }));
});
