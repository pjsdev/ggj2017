using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFSM 
{
	public interface State 
	{
		void Update ();
		void Enter ();
		void Exit ();
	}

	public class StateMachine : MonoBehaviour
	{
		State Current;

		public System.Type CurrentState()
		{
			return Current.GetType();
		}

		Dictionary<System.Type, State> States;

		public StateMachine()
		{
			States = new Dictionary<System.Type, State> ();
		}

		public void AddState(State _state)
		{
			States[_state.GetType()] = _state;
		}

		public void Enter<T>() where T: State
		{
			var newType = typeof(T);

			// only call end if we have a currentState
			if (Current != null)
			{
				// avoid changing to the same state
				if (Current.GetType () == newType)
					return;

				Debug.LogFormat ("Leaving state: {0}", Current.GetType().ToString ());
				Current.Exit ();
			}

			#if UNITY_EDITOR
			// do a sanity check while in the editor to ensure we have the given state in our state list
			if(!States.ContainsKey(newType))
			{
				var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
				Debug.LogError( error );
				throw new System.Exception( error );
			}
			#endif

			// swap states and call begin
			Current = States[newType];
			Debug.LogFormat ("Entering state: {0}", Current.GetType().ToString ());
			Current.Enter();
		}

		public void Update()
		{
			if(Current == null)
			{
				Debug.LogError ("Could not find state to update...");	
				return;
			}
	
			Current.Update ();
		}
	}
}
