import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RegistrationComponent } from './registration.component';
import { RegistrationService } from '../services/registration.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('RegistrationComponent', () => {
  let component: RegistrationComponent;
  let fixture: ComponentFixture<RegistrationComponent>;
  let mockRegistrationService: jasmine.SpyObj<RegistrationService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockRegistrationService = jasmine.createSpyObj('RegistrationService', ['registerUser']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [RegistrationComponent],
      providers: [
        { provide: RegistrationService, useValue: mockRegistrationService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show password error when passwords do not match', () => {
    component.user.password = 'password1';
    component.user.confirmPassword = 'password2';

    component.onRegister();

    expect(component.showPasswordError).toBeTrue();
    expect(mockRegistrationService.registerUser).not.toHaveBeenCalled();
  });

  it('should register successfully and navigate to homepage', fakeAsync(() => {
    const mockResponse = { userId: '123', isAdmin: false };

    component.user.email = 'test@example.com';
    component.user.password = 'password';
    component.user.confirmPassword = 'password';

    mockRegistrationService.registerUser.and.returnValue(of(mockResponse));

    component.onRegister();

    expect(component.showSuccessMessage).toBeTrue();
    expect(localStorage.getItem('userId')).toBe('123');
    expect(localStorage.getItem('isAdmin')).toBe('false');
    expect(localStorage.getItem('isLogged')).toBe('true');

    tick(3000);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/homepage']);
  }));

  it('should show email error when email is already in use', () => {
    component.user.email = 'test@example.com';
    component.user.password = 'password';
    component.user.confirmPassword = 'password';

    const errorResponse = {
      error: {
        message: 'Email already in use'
      }
    };

    mockRegistrationService.registerUser.and.returnValue(throwError(() => errorResponse));

    spyOn(window, 'alert');

    component.onRegister();

    expect(component.showEmailError).toBeTrue();
    expect(component.showPasswordError).toBeFalse();
    expect(component.showSuccessMessage).toBeFalse();
  });

  it('should alert for unknown error', () => {
    component.user.email = 'test@example.com';
    component.user.password = 'password';
    component.user.confirmPassword = 'password';

    const errorResponse = {
      error: {
        message: 'Some unknown error'
      }
    };

    mockRegistrationService.registerUser.and.returnValue(throwError(() => errorResponse));

    const alertSpy = spyOn(window, 'alert');

    component.onRegister();

    expect(alertSpy).toHaveBeenCalledWith('Some unknown error');
    expect(component.showEmailError).toBeFalse();
    expect(component.showPasswordError).toBeFalse();
    expect(component.showSuccessMessage).toBeFalse();
  });

});
