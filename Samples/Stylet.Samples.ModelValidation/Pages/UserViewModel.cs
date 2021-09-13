using FluentValidation;
using Stylet.Samples.ModelValidation.Models;
using System;
using System.Collections.Generic;

namespace Stylet.Samples.ModelValidation.Pages
{
    public class UserViewModel : Screen
    {
        private IWindowManager windowManager;
        private UserModel _userModel = new UserModel();

        public bool ShouldAutoValidate
        {
            get { return base.AutoValidate; }
            set
            {
                base.AutoValidate = value;
                this.NotifyOfPropertyChange();
            }
        }

        public UserModel UserModel
        {
            get => _userModel;
            set => SetAndNotify(ref _userModel, value);
        }

        public UserViewModel(IWindowManager windowManager, IModelValidator<UserViewModel> validator) : base(validator)
        {
            this.windowManager = windowManager;
            // Force initial validation
            this.Validate();
            // Whenever password changes, we need to re-validate PasswordConfirmation
            this.Bind(x => x.UserModel.Password, (o, e) => this.ValidateProperty(() => this.UserModel.PasswordConfirmation));
        }

        protected override void OnValidationStateChanged(IEnumerable<string> changedProperties)
        {
            base.OnValidationStateChanged(changedProperties);
            // Fody can't weave other assemblies, so we have to manually raise this
            this.NotifyOfPropertyChange(() => this.CanSubmit);
        }

        public bool CanSubmit
        {
            get { return !this.ShouldAutoValidate || !this.HasErrors; }
        }
        public void Submit()
        {
            if (this.Validate())
            {
                this.windowManager.ShowMessageBox("Successfully submitted", "success");
            }
        }
    }

    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator(UserModelValidator modelValidator)
        {
            RuleFor(x => x.UserModel).SetValidator(modelValidator);
        }
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Length(1, 20);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches("[0-9]").WithMessage("{PropertyName} must contain a number");
            RuleFor(x => x.PasswordConfirmation).Equal(s => s.Password).WithMessage("{PropertyName} should match Password");
            RuleFor(x => x.Age).Must(x => x.IsValid).WithMessage("{PropertyName} must be a valid number");
            When(x => x.Age.IsValid, () =>
            {
                RuleFor(x => x.Age.Value).GreaterThan(0).WithName("Age");
            });
        }
    }
}
