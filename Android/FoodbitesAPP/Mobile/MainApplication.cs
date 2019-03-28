using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;

namespace FoodbitesAPP
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public UserModel CurrentUser { get; set; }
        public List<PetiscoModel> ListaPetiscos { get; set; }
        public PetiscoModel CurrentPetisco { get; set; }


        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
            CurrentUser = new UserModel();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public void SetPetiscos(List<PetiscoModel> lista) {
            if(ListaPetiscos != null) 
            {
				foreach (PetiscoModel p in ListaPetiscos)
				{
                    if (p.JaTemImagem)
                    {
                        p.JaTemImagem = false;
						if (p.ImageBitmap != null)
						{
							p.ImageBitmap.Recycle();
							p.ImageBitmap.Dispose();
							p.ImageBitmap = null;
                            
                        }
                    }
				}
            }
            ListaPetiscos = lista;
        }



    }
}