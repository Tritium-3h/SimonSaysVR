
public class SimonEvent  {

	public string color;
	public string counter;

	public SimonEvent(string color, string counter) {
		this.color = color;
		this.counter = counter;
	}

	public SimonEvent(string color, long counter) {
		this.color = color;
		this.counter = ""+counter;
	}

}
