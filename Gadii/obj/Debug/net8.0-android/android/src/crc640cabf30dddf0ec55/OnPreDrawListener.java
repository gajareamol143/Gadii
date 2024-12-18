package crc640cabf30dddf0ec55;


public class OnPreDrawListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.ViewTreeObserver.OnPreDrawListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPreDraw:()Z:GetOnPreDrawHandler:Android.Views.ViewTreeObserver/IOnPreDrawListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("UraniumUI.Blurs.OnPreDrawListener, UraniumUI.Blurs", OnPreDrawListener.class, __md_methods);
	}


	public OnPreDrawListener ()
	{
		super ();
		if (getClass () == OnPreDrawListener.class) {
			mono.android.TypeManager.Activate ("UraniumUI.Blurs.OnPreDrawListener, UraniumUI.Blurs", "", this, new java.lang.Object[] {  });
		}
	}


	public boolean onPreDraw ()
	{
		return n_onPreDraw ();
	}

	private native boolean n_onPreDraw ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
