require "System.Global"

class ("Login")

function Login:Awake(this)

	self.this = this
	
	Login.Instance = self

	--self.testBtn = self.this.transform:Find("Test")
	--self.mid = self.this.transform:Find("Mid")
end
	
function Login:New()
	print("Login:New")
end