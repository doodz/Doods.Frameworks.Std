// ---------------------------------------------------------------------------
//  This source file is the confidential property and copyright of WIUZ.
//  Reproduction or transmission in whole or in part, in any form or
//  by any means, electronic, mechanical or otherwise, is prohibited
//  without the prior written consent of the copyright owner.
//  <copyright file="ShellNavigationService.cs" company="WIUZ">
//     Copyright (C) WIUZ.  All rights reserved. 2016 - 2019
//  </copyright>
//  History:
//   2019/07/27 at 14:29:  aka therv.
// ---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Doods.Framework.Mobile.Std.Interfaces;
using Doods.Framework.Std;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Servicies
{
    public class ShellNavigationService : NavigationBaseService,INavigationService
    {
        public ShellNavigationService(ILogger logger, ITelemetryService telemetryService):base(logger, telemetryService)
        {

        }

        public override void Configure(string pageKey, Type pageType)
        {
           base.Configure(pageKey,pageType);
           Routing.RegisterRoute(pageKey, pageType);
        }

        public  Task GoBack()
        {
           
            return Shell.Current.Navigation.PopAsync();
        }

        public  Task GoToRootAsync()
        {
            return Shell.Current.Navigation.PopToRootAsync();
        }

        public Task NavigateModalAsync(string pageKey, bool animated = true)
        {
            throw new NotImplementedException();
        }

        public Task NavigateModalAsync(string pageKey, object parameter, bool animated = true)
        {
            throw new NotImplementedException();
            
           
        }


        

        public async Task NavigateAsync(string pageKey, bool animated = true)
        {
            var state = Shell.Current.CurrentState;
            //await Shell.Current.GoToAsync($"{state.Location}/{pageKey}", animated);
            await Shell.Current.GoToAsync($"{pageKey}", animated);
            Shell.Current.FlyoutIsPresented = false;
        }

        public async Task NavigateAsync(string pageKey, object parameter, bool animated = true)
        {
            if (parameter is IQueryShellNavigationObject shellNavigationObject)
            {
                
                var state = Shell.Current.CurrentState;
                //await Shell.Current.GoToAsync($"{state.Location}/{pageKey}?{shellNavigationObject.ToQuery()}", animated);
                await Shell.Current.GoToAsync($"{pageKey}?{shellNavigationObject.ToQuery()}", animated);
                Shell.Current.FlyoutIsPresented = false;
            }
            else
            {
                throw new InvalidOperationException($"You need pass a {nameof(IQueryShellNavigationObject)}");
            }
        }
    }
}