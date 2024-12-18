package crc640cabf30dddf0ec55;


public class BlurViewCanvas
	extends android.graphics.Canvas
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("UraniumUI.Blurs.BlurViewCanvas, UraniumUI.Blurs", BlurViewCanvas.class, __md_methods);
	}


	public BlurViewCanvas ()
	{
		super ();
		if (getClass () == BlurViewCanvas.class) {
			mono.android.TypeManager.Activate ("UraniumUI.Blurs.BlurViewCanvas, UraniumUI.Blurs", "", this, new java.lang.Object[] {  });
		}
	}


	public BlurViewCanvas (android.graphics.Bitmap p0)
	{
		super (p0);
		if (getClass () == BlurViewCanvas.class) {
			mono.android.TypeManager.Activate ("UraniumUI.Blurs.BlurViewCanvas, UraniumUI.Blurs", "Android.Graphics.Bitmap, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}

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
